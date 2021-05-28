<template>
  <div class="login">
    <el-card class="box-card form">
      <div class="title-container">
        <h3 class="title">Proyecto Final</h3>
        <h4 class="title">Inicio de Sesion</h4>
      </div>

      <el-form
        :model="model"
        :rules="validaciones"
        ref="form1"
        label-width="140px"
      >
        <el-form-item label="Usuario" prop="usuario">
          <el-input v-model="model.usuario"></el-input>
        </el-form-item>
        <el-form-item label="Contraseña" prop="clave">
          <el-input type="password" v-model="model.clave"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="ingresar()">Ingresar</el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script>
// @ is an alias to /src

export default {
  name: 'Home',
  data: () => ({
    model: {
      usuario: null,
      clave: null
    },
    validaciones: {
      usuario: [
        {
          required: true,
          message: 'Por favor ingresar su usuario',
          trigger: 'blur'
        }
      ],
      clave: [
        {
          required: true,
          message: 'Por favor ingresar su contraseña',
          trigger: 'blur'
        }
      ]
    }
  }),
  methods: {
    ingresar() {
      this.$refs.form1.validate(async (valid) => {
        if (valid) {
          const loading = this.$loading({
            lock: true,
            text: 'Cargando',
            spinner: 'el-icon-loading',
            background: 'rgba(0, 0, 0, 0.3)'
          })
          try {
            await this.$store.dispatch('iniciarSesion', this.model)
            this.$router.push('/')
          } catch (error) {
            this.$MensajeError(error)
          } finally {
            loading.close()
          }
        }
      })
    }
  }
}
</script>

<style lang="scss" scoped>
.login {
  min-height: 100%;
  width: 100%;
  overflow: hidden;
  .form {
    position: relative;
    width: 520px;
    max-width: 100%;
    padding: 20px 35px 20px 0px;
    margin: 0 auto;
    margin-top: 160px;
    overflow: hidden;
  }
  .title-container {
    position: relative;
    color: #5e645e;
    h3.title {
      font-size: 2rem;
      margin: 0px auto 0px auto;
      text-align: center;
      font-weight: bold;
    }
    h4.title {
      font-size: 1.5rem;
      margin: 10px auto 20px auto;
      text-align: center;
      font-weight: bold;
    }
  }
}
</style>
