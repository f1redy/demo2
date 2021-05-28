import Vue from 'vue'
import VueRouter from 'vue-router'

import Layout from '../views/Layout'

Vue.use(VueRouter)

const routes = [
  {
    path: '/login',
    component: () => import('../views/Login'),
    meta: { login: true }
  },
  {
    path: '/',
    component: Layout,
    children: [
      {
        path: '/',
        component: () => import('../views/Home')
      },
      {
        path: '/maestro/area',
        component: () => import('../views/maestro/Area')
      },
      {
        path: '/maestro/tarea',
        component: () => import('../views/maestro/Tarea')
      },
      {
        path: '/maestro/solicitante',
        component: () => import('../views/maestro/Solicitante')
      },
      {
        path: '/maestro/especialista',
        component: () => import('../views/maestro/Especialista')
      },
      {
        path: '/gestion/exportar',
        component: () => import('../views/gestiones/Exportacion')
      },
      {
        path: '/gestion/asignacion',
        component: () => import('../views/gestiones/AsignacionTarea')
      },
      {
        path: '/gestion/modificacion',
        component: () => import('../views/gestiones/ModificacionTarea')
      },
      {
        path: '/reportes/por-criticidad',
        component: () => import('../views/reportes/PorCriticidad')
      },
      {
        path: '/reportes/por-responsable',
        component: () => import('../views/reportes/PorResponsable')
      },
      {
        path: '/reportes/por-area',
        component: () => import('../views/reportes/PorArea')
      },
      {
        path: '/reportes/en-tiempo',
        component: () => import('../views/reportes/EnTiempo')
      },
      {
        path: '/reportes/fuera-de-tiempo',
        component: () => import('../views/reportes/FueraTiempo')
      },
      {
        path: '*',
        component: () => import('../views/404')
      }
    ]
  }
]

const router = new VueRouter({
  mode: 'history',
  routes
})

router.beforeEach((to, from, next) => {
  const { login } = to.meta
  const currentUser = JSON.parse(localStorage.getItem('usuario'))
  if (login) {
    if (currentUser) {
      return next({ path: '/' })
    }
  } else {
    if (!currentUser) {
      return next({ path: '/login' })
    }
  }
  next()
})
export default router
