<template>
  <div class="mante w-100">
    <el-card>
      <h3 class="gris titulo">Reporte Por Area</h3>
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
        <el-form-item label="Criticidad">
          <select-search url="api/consultas/areas" v-model="area_id" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" plain @click="cargarInfo">
            Ver Informacion
          </el-button>
          <el-button type="primary" plain @click="DescargarDatos">
            Descargar Datos
          </el-button>
        </el-form-item>
      </el-form>
      <el-table :data="datos" style="width: 100%" size="mini" height="400">
        <el-table-column prop="area_id" label="Id" width="100" sortable />
        <el-table-column prop="area_desc" label="Nombre" sortable />
        <el-table-column
          prop="total"
          label="Total"
          width="150"
          sortable
          align="right"
        />
        <el-table-column
          prop="solucionada"
          label="Solucionados"
          width="150"
          sortable
          align="right"
        />
        <el-table-column
          prop="anulada"
          label="Anulados"
          width="150"
          sortable
          align="right"
        />
        <el-table-column
          prop="en_proceso"
          label="En Proceso"
          width="150"
          sortable
          align="right"
        />
        <el-table-column
          prop="registrada"
          label="Registradas"
          width="150"
          sortable
          align="right"
        />
        <el-table-column
          prop="asignada"
          label="Asignadas"
          width="150"
          sortable
          align="right"
        />
      </el-table>
      <download-csv :data="datos" ref="dw" v-show="false"> </download-csv>
    </el-card>
  </div>
</template>

<script>
export default {
  data: () => ({
    valor: [],
    area_id: null,
    datos: [],
    url: '/api/reportes/por-area'
  }),
  methods: {
    async cargarInfo() {
      if (!this.valor || this.valor.length == 0) {
        this.$notify.error('Favor ingresar rango de fechas')
        return
      }
      var area_id = this.area_id
      if (!area_id) {
        area_id = '*'
      }
      const loading = this.$loading({
        lock: true,
        text: 'Cargando',
        spinner: 'el-icon-loading',
        background: 'rgba(0, 0, 0, 0.3)'
      })
      try {
        let ret = await this.$http.get(
          this.url + '/' + area_id + '/' + this.valor[0] + '/' + this.valor[1]
        )
        if (ret) {
          this.datos = ret.data
        }
        // await this.$nextTick()
        // if (!this.datos || this.datos.length == 0) {
        //   this.$MensajeError('No hay datos')
        // }
        // this.$refs.dw.generate()
      } catch (error) {
        this.$MensajeError(error)
      } finally {
        loading.close()
      }
    },
    async DescargarDatos() {
      if (!this.datos || this.datos.length == 0) {
        this.$MensajeError('No hay datos')
      }
      this.$refs.dw.generate()
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
