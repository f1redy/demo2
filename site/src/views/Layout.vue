<template>
  <el-container class="h-100">
    <el-header height="50px" class="border b-white">
      <div class="navbar">
        <div class="title">Proyecto FINAL</div>
        <div class="right-menu">
          <el-dropdown
            class="avatar-container right-menu-item hover-effect"
            trigger="click"
          >
            <div class="avatar-wrapper">
              {{ usuario.nombre }}
              <i class="el-icon-caret-bottom" />
            </div>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item @click.native="cerrarSesion">
                <span>Cerrar Sesion</span>
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
        </div>
      </div>
    </el-header>

    <el-container class="h-100">
      <el-aside width="250px"> <MenuPrincipal /> </el-aside>

      <el-container class="w-100"> <router-view /> </el-container>
    </el-container>
  </el-container>
</template>

<script>
import MenuPrincipal from '@/components/MenuPrincipal'
import { mapState } from 'vuex'
export default {
  components: {
    MenuPrincipal
  },
  methods: {
    cerrarSesion() {
      this.$store.dispatch('cerrarSesion')
      this.$router.push('/login')
    }
  },
  computed: {
    ...mapState(['usuario'])
  }
}
</script>

<style lang="scss" scoped>
.border {
  box-shadow: 0 1px 4px rgba(0, 21, 41, 0.08);
  z-index: 10;
}
.b-white {
  background: #ffffff;
}
.navbar {
  height: 50px;
  overflow: hidden;
  position: relative;
  line-height: 50px;
  background: #ffffff;
  .title {
    height: 100%;
    float: left;
  }

  .right-menu {
    float: right;
    height: 100%;

    .right-menu-item {
      display: inline-block;
      padding-right: 10px;
      padding-left: 10px;
      vertical-align: text-bottom;
      &.hover-effect {
        cursor: pointer;
        transition: background 0.3s;
        &:hover {
          background: rgba(0, 0, 0, 0.015);
        }
      }
    }
  }
}
</style>
