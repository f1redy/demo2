<template>
  <div class="mante w-100">
    <el-card>
      <div>
        <h3 class="gris titulo">Asignacion Tarea</h3>
        <div class="opciones">
          <el-select v-model="tipo" placeholder="" size="mini">
            <el-option key="1" label="Sin Asignar" :value="1" />
            <el-option key="2" label="En Proceso" :value="2" />
          </el-select>
          <el-button
            size="mini"
            icon="el-icon-refresh"
            plain
            @click="cargarInfo"
            >Ver</el-button
          >
        </div>
      </div>
      <el-table :data="datos" style="width: 100%" size="mini">
        <el-table-column prop="tarea_id" label="Id" width="100" sortable />
        <el-table-column prop="tarea_desc" label="Nombre" sortable />
        <el-table-column
          prop="criticidad_desc"
          label="Criticidad"
          width="100"
          sortable
        />
        <el-table-column
          prop="especialista_desc"
          label="Especialista"
          width="130"
          sortable
        />
        <el-table-column
          prop="estado_desc"
          label="Estado"
          width="100"
          sortable
        />
        <el-table-column label="Acciones" width="100" align="center">
          <template slot-scope="scope">
            <el-button type="text" class="p-0" @click="ver(scope.row)"
              >Asignar</el-button
            >
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog
      title="Asignar Responsable"
      :visible.sync="ventanaDetalle"
      width="80%"
      center
    >
      <div class="detalle">
        <div class="row">
          <div>Id</div>
          <div>{{ model.tarea_id }}</div>
        </div>
        <div class="row">
          <div>Descripcion</div>
          <div>{{ model.tarea_desc }}</div>
        </div>
        <div class="row">
          <div>Area</div>
          <div>{{ model.area_desc }}</div>
        </div>
        <div class="row">
          <div>Criticidad</div>
          <div>{{ model.criticidad_desc }}</div>
        </div>
        <div class="row">
          <div>Estado</div>
          <div>{{ model.estado_desc }}</div>
        </div>
        <div class="row">
          <div>Solicitante</div>
          <div>{{ model.solicitante_id + ' - ' + model.solicitante_desc }}</div>
        </div>
        <div class="row">
          <div>Creado Por</div>
          <div>{{ model.creador_id + ' - ' + model.creador_desc }}</div>
        </div>
        <div class="row">
          <div>Fecha Solicitud</div>
          <div>{{ model.fecha_solicitud | moment('DD-MM-YYYY') }}</div>
        </div>
        <div class="row">
          <div>Fecha Compromiso</div>
          <div>{{ model.fecha_compromiso | moment('DD-MM-YYYY') }}</div>
        </div>
        <div class="row">
          <div>Fecha Asignacion</div>
          <div>{{ model.fecha_asignacion | moment('DD-MM-YYYY') }}</div>
        </div>

        <div class="row" v-for="(x, i) in model.especialistas" :key="i">
          <div><span v-if="i == 0">Especialistas</span></div>
          <div>{{ x.especialista_id + ' - ' + x.especialista_desc }}</div>
        </div>
        <div class="row mt-2">
          <div class="mt-1">Asignar</div>
          <div class="w-r-100">
            <select-search
              url="api/consultas/especialistas"
              v-model="asignacion.especialista_id"
              class="w-r-100 float-left"
              size="mini"
            />
          </div>
          <div class="mt-1">
            <el-button
              type="primary"
              icon="el-icon-plus"
              circle
              class="float-right"
              size="mini"
              @click="asignar"
            ></el-button>
          </div>
        </div>
      </div>
      <span slot="footer" class="dialog-footer">
        <el-button @click="ventanaDetalle = false">Cancelar</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
const requerido = {
  required: true,
  message: 'Por favor ingresar dato',
  trigger: 'blur'
}
export default {
  data: () => ({
    model: {
      tarea_id: 0,
      tarea_desc: null,
      area_desc: null,
      area_id: null,
      creador_desc: null,
      creador_id: 0,
      criticidad_desc: null,
      criticidad_id: null,
      estado_desc: null,
      estado_id: null,
      fecha_asignacion: null,
      fecha_compromiso: null,
      fecha_solicitud: null,
      fecha_solucion: null,
      solicitante_desc: null,
      solicitante_id: null
    },
    asignacion: {
      tarea_id: null,
      especialista_id: null
    },
    tipo: 1,
    ventanaDetalle: false,
    tipoDetalle: true,
    ventanaNuevo: false,
    tipoNuevo: true,
    validaciones: {
      tarea_desc: [requerido],
      area_id: [requerido],
      criticidad_id: [requerido],
      estado_id: [requerido],
      fecha_compromiso: [requerido],
      solicitante_id: [requerido]
    },
    datos: [],
    url: '/api/gestiones/asignaciontarea'
  }),
  mounted() {
    this.cargarInfo()
  },
  methods: {
    ver(row, val) {
      this.tipoDetalle = val
      this.ventanaDetalle = true
      this.cargarRegistro(row.tarea_id)
    },
    async asignar() {
      if (!this.asignacion.especialista_id) {
        this.$MensajeError('Se debe seleccionar un especialista')
        return
      }
      const loading = this.$loading({
        lock: true,
        text: 'Cargando',
        spinner: 'el-icon-loading',
        background: 'rgba(0, 0, 0, 0.3)'
      })

      try {
        this.asignacion.tarea_id = this.model.tarea_id
        await this.$http.post(this.url, this.asignacion)

        await this.cargarRegistro(this.model.tarea_id)
        await this.cargarInfo()
      } catch (error) {
        this.$MensajeError(error)
      } finally {
        loading.close()
      }
    },
    async cargarRegistro(id) {
      const loading = this.$loading({
        lock: true,
        text: 'Cargando',
        spinner: 'el-icon-loading',
        background: 'rgba(0, 0, 0, 0.3)'
      })

      try {
        let ret = await this.$http.get(this.url + '/' + id)
        if (ret) {
          this.model = ret.data
        }
      } catch (error) {
        this.$MensajeError(error)
      } finally {
        loading.close()
      }
    },
    async cargarInfo() {
      const loading = this.$loading({
        lock: true,
        text: 'Cargando',
        spinner: 'el-icon-loading',
        background: 'rgba(0, 0, 0, 0.3)'
      })
      var tipo = this.tipo === 1 ? '/sin-asignar' : '/en-proceso'
      try {
        let ret = await this.$http.get(this.url + tipo)
        if (ret) {
          this.datos = ret.data
        }
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
.titulo {
  float: left;
}

.opciones {
  float: right;
  margin-top: 1rem;
}
.row {
  margin-bottom: 0.2rem;
  display: flex;
  div:first-child {
    float: left;
    width: 140px;
    color: rgb(124, 124, 124);
  }
  div:last-child {
    position: relative;
    color: rgb(105, 103, 103);
    font-weight: 600;
  }
}
.w-r-100 {
  width: 30rem !important;
}
</style>
