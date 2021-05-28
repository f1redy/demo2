<template>
  <div class="mante w-100">
    <el-card>
      <h3 class="gris titulo">Exportacion de Gestiones</h3>
      <el-form label-width="150px" size="mini">
        <el-form-item label="Fechas">
          <el-date-picker
            v-model="valor"
            type="daterange"
            range-separator="To"
            start-placeholder=" Inicio"
            end-placeholder=" Final"
            format="dd-MM-yyyy"
            value-format="yyyy-MM-dd"
          >
          </el-date-picker>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" plain @click="cargarInfo">
            Descargar Datos
          </el-button>
        </el-form-item>
      </el-form>

      <download-csv :data="datos" ref="dw" v-show="false"> </download-csv>
    </el-card>
  </div>
</template>

<script>
export default {
  data: () => ({
    valor: [],
    model: {
      fechai: null,
      fechaf: null
    },
    datos: [],
    url: '/api/gestiones/exportacion'
  }),
  methods: {
    async cargarInfo() {
      if (!this.valor || this.valor.length == 0) {
        this.$notify.error('Favor ingresar rango de fechas')
        return
      }
      const loading = this.$loading({
        lock: true,
        text: 'Cargando',
        spinner: 'el-icon-loading',
        background: 'rgba(0, 0, 0, 0.3)'
      })
      try {
        let ret = await this.$http.get(
          this.url + '/' + this.valor[0] + '/' + this.valor[1]
        )
        if (ret) {
          this.datos = ret.data
        }

        await this.$nextTick()

        if (!this.datos || this.datos.length == 0) {
          this.$MensajeError('No hay datos')
        }
        this.$refs.dw.generate()
      } catch (error) {
        this.$MensajeError(error)
      } finally {
        loading.close()
      }
    }
  }
}
</script>
<style lang="scss" scoped>
.mante {
  padding: 1rem;
}
.gris {
  color: #6d6d6d !important;
}
.rojo {
  color: #e45454 !important;
}
.verde {
  font-weight: 500;
}
</style>
