import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'
const http = axios.create({
  baseURL: '/',
  timeout: 0
})
Vue.use(Vuex)

const usuario = JSON.parse(localStorage.getItem('usuario'))
const estado = usuario
  ? { logueado: true, usuario }
  : { logueado: false, usuario: null }

export default new Vuex.Store({
  state: estado,
  mutations: {
    setUsuario(state, user) {
      if (user) {
        state.usuario = user
        state.logueado = true
      } else {
        state.usuario = null
        state.logueado = false
      }
    }
  },
  actions: {
    async iniciarSesion({ commit }, usuario) {
      try {
        const ret = await http.post('/api/site/login', {
          usuario: usuario.usuario,
          clave: usuario.clave
        })
        if (ret.data.token) {
          localStorage.setItem('usuario', JSON.stringify(ret.data))
        }
        commit('setUsuario', ret.data)

        Vue.prototype.$http.defaults.headers.common['Authorization'] =
          'bearer ' + ret.data.token

        return Promise.resolve(ret.data)
      } catch (error) {
        commit('setUsuario', null)
        return Promise.reject(error)
      }
    },
    cerrarSesion({ commit }) {
      localStorage.removeItem('usuario')
      commit('setUsuario', null)
    }
  }
})
