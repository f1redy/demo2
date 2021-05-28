import Vue from 'vue'
import App from './App.vue'
import router from './router'
import JsonCSV from 'vue-json-csv'
import Element from 'element-ui'
import '@/styles/main.scss'
import locale from 'element-ui/lib/locale/lang/es'
import 'normalize.css'
import axios from 'axios'
import SelectSearch from '@/components/SelectSearch'
import store from './store'
const serviceAxios = axios.create({
  baseURL: '/',
  timeout: 0
})
Vue.component('download-csv', JsonCSV)
Vue.use(require('vue-moment'))
serviceAxios.interceptors.request.use(
  (config) => {
    config.withCredentials = true
    return config
  },
  (error) => {
    Promise.reject(error)
  }
)
const usuario = JSON.parse(localStorage.getItem('usuario'))

Vue.prototype.$http = serviceAxios
if (usuario) {
  Vue.prototype.$http.defaults.headers.common['Authorization'] =
    'bearer ' + usuario.token
}
function MensajeError(error) {
  if (
    error &&
    error.response &&
    (error.response.status === 403 || error.response.status === 401)
  ) {
    this.$notify.error({
      title: 'Error',
      message: 'Acceso no permitido'
    })
  } else if (error && error.response && error.response.data) {
    this.$notify.error({
      title: 'Error',
      message:
        error.response.data.title ||
        error.response.data.Message ||
        error.response.data
    })
  } else {
    if (error !== 'cancel') {
      this.$notify.error({
        title: 'Error',
        message: error
      })
    }
  }
}
Vue.prototype.$MensajeError = MensajeError
Vue.config.productionTip = false

Vue.component('select-search', SelectSearch)
Vue.use(Element, { locale })

new Vue({
  router,
  store,
  render: (h) => h(App)
}).$mount('#app')
