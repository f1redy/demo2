<template>
  <div class="mante w-100">
    <el-card>
      <h3 class="gris titulo">Reporte Por Criticidad</h3>
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
          <select-search
            url="api/consultas/criticidad"
            v-model="criticidad_id"
          />
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
        <el-table-column prop="tarea_id" label="Id" width="100" sortable />
        <el-table-column
          prop="tarea_desc"
          label="Descripcion"
          sortable
          width="450"
        />
        <el-table-column prop="area_desc" label="Area" width="150" sortable />
        <el-table-column
          prop="creador_desc"
          label="Creador Por"
          width="150"
          sortable
        />
        <el-table-column
          prop="criticidad_desc"
          label="Criticidad"
          width="150"
          sortable
        />
        <el-table-column
          prop="estado_desc"
          label="Estado"
          width="150"
          sortable
        />
        <el-table-column
          prop="solicitante_desc"
          label="Solicitante"
          width="150"
          sortable
        />
        <el-table-column
          prop="especialistas"
          label="Especialista"
          width="200"
          sortable
        />
        <el-table-column label="F Solicitud" width="200" prop="fecha_solicitud">
          <template slot-scope="scope">
            {{ scope.row.fecha_solicitud | moment('DD-MM-YYYY LTS') }}
          </template>
        </el-table-column>
        <el-table-column
          label="F Compromiso"
          width="200"
          prop="fecha_compromiso"
        >
          <template slot-scope="scope">
            {{ scope.row.fecha_compromiso | moment('DD-MM-YYYY LTS') }}
          </template>
        </el-table-column>
        <el-table-column
          label="F Asignacion"
          width="200"
          prop="fecha_asignacion"
        >
          <template slot-scope="scope">
            {{ scope.row.fecha_asignacion | moment('DD-MM-YYYY LTS') }}
          </template>
        </el-table-column>
        <el-table-column label="F Solucion" width="200" prop="fecha_solucion">
          <template slot-scope="scope">
            {{ scope.row.fecha_solucion | moment('DD-MM-YYYY LTS') }}
          </template>
        </el-table-column>
      </el-table>
      <download-csv :data="datos" ref="dw" v-show="false"> </download-csv>
    </el-card>
  </div>
</template>

<script>
export default {
  data: () => ({
    valor: [],
    criticidad_id: null,
    datos: [],
    url: '/api/reportes/por-criticidad'
  }),
  methods: {
    async cargarInfo() {
      if (!this.valor || this.valor.length == 0) {
        this.$notify.error('Favor ingresar rango de fechas')
        return
      }
      var criticidad = this.criticidad_id
      if (!criticidad) {
        criticidad = '*'
      }
      const loading = this.$loading({
        lock: true,
        text: 'Cargando',
        spinner: 'el-icon-loading',
        background: 'rgba(0, 0, 0, 0.3)'
      })
      try {
        let ret = await this.$http.get(
          this.url +
            '/' +
            criticidad +
            '/' +
            this.valor[0] +
            '/' +
            this.valor[1]
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
