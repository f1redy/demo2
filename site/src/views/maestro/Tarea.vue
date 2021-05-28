<template>
  <div class="mante w-100">
    <el-card>
      <div>
        <h3 class="gris titulo">Tareas</h3>
        <div class="opciones">
          <el-button
            size="mini"
            icon="el-icon-refresh"
            plain
            @click="cargarInfo"
            >Recargar</el-button
          >
          <el-button
            size="mini"
            icon="el-icon-plus"
            plain
            @click="nuevo(null, true)"
            >Nuevo</el-button
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
            <el-button
              type="text"
              icon="el-icon-view"
              class="gris p-0"
              @click="ver(scope.row, true)"
            ></el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog
      :title="
        tipoDetalle ? 'Detalle Registro' : 'Seguro de eliminar el registro?'
      "
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
        <div class="row">
          <div>Fecha Solucion</div>
          <div>{{ model.fecha_solucion | moment('DD-MM-YYYY') }}</div>
        </div>
        <div class="row">
          <div>Especialistas</div>
          <div></div>
        </div>
        <div class="row" v-for="(x, i) in model.especialistas" :key="i">
          <div></div>
          <div>{{ x.especialista_id + ' - ' + x.especialista_desc }}</div>
        </div>
      </div>
      <span slot="footer" class="dialog-footer">
        <el-button @click="ventanaDetalle = false">Cancelar</el-button>
        <el-button
          type="danger"
          @click="eliminarRegistro(model.tarea_id)"
          v-if="!tipoDetalle"
          >Eliminar</el-button
        >
      </span>
    </el-dialog>

    <el-dialog
      :title="tipoNuevo ? 'Nuevo Registro' : 'Modificar Registro'"
      :visible.sync="ventanaNuevo"
      width="80%"
      center
      :close-on-click-modal="false"
      :close-on-press-escape="false"
    >
      <el-form
        :model="model"
        :rules="validaciones"
        ref="form1"
        label-width="150px"
        size="mini"
      >
        <el-form-item label="Id" prop="tarea_id">
          <el-input
            v-model="model.tarea_id"
            readonly
            :disabled="tipoNuevo"
          ></el-input>
        </el-form-item>
        <el-form-item label="Descripcion" prop="tarea_desc">
          <el-input v-model="model.tarea_desc"></el-input>
        </el-form-item>
        <el-form-item label="Area" prop="area_id">
          <select-search url="api/consultas/areas" v-model="model.area_id" />
        </el-form-item>
        <el-form-item label="Criticidad" prop="criticidad_id">
          <select-search
            url="api/consultas/criticidad"
            v-model="model.criticidad_id"
          />
        </el-form-item>
        <el-form-item label="Estado" prop="estado_id">
          <select-search
            url="api/consultas/estado-inicial"
            v-model="model.estado_id"
          />
        </el-form-item>
        <el-form-item label="Solicitante" prop="solicitante_id">
          <select-search
            url="api/consultas/solicitantes"
            v-model="model.solicitante_id"
          />
        </el-form-item>
        <el-form-item label="Fecha Compromiso" prop="fecha_compromiso">
          <el-date-picker
            v-model="model.fecha_compromiso"
            type="date"
            placeholder=""
            format="dd-MM-yyyy"
          >
          </el-date-picker>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="ventanaNuevo = false">Cancelar</el-button>
        <el-button type="primary" @click="agregarRegistro()">Agragar</el-button>
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
    url: '/api/maestros/tarea'
  }),
  mounted() {
    this.cargarInfo()
  },
  methods: {
    async nuevo(row, val) {
      this.tipoNuevo = val
      this.ventanaNuevo = true
      if (val) {
        this.model.tarea_id = 0
        this.model.tarea_desc = null
        this.model.area_desc = null
        this.model.area_id = null
        this.model.creador_desc = null
        this.model.creador_id = 0
        this.model.criticidad_desc = null
        this.model.criticidad_id = null
        this.model.estado_desc = null
        this.model.estado_id = null
        this.model.fecha_asignacion = null
        this.model.fecha_compromiso = null
        this.model.fecha_solicitud = null
        this.model.fecha_solucion = null
        this.model.solicitante_desc = null
        this.model.solicitante_id = null
      } else {
        await this.cargarRegistro(row.tarea_id)
      }
    },
    ver(row, val) {
      this.tipoDetalle = val
      this.ventanaDetalle = true
      this.cargarRegistro(row.tarea_id)
    },
    editarRegistro() {
      this.$refs.form1.validate(async (valid) => {
        if (valid) {
          const loading = this.$loading({
            lock: true,
            text: 'Cargando',
            spinner: 'el-icon-loading',
            background: 'rgba(0, 0, 0, 0.3)'
          })
          try {
            await this.$http.put(
              this.url + '/' + this.model.tarea_id,
              this.model
            )
            this.ventanaNuevo = false
            await this.cargarInfo()
          } catch (error) {
            this.$MensajeError(error)
          } finally {
            loading.close()
          }
        }
      })
    },
    agregarRegistro() {
      this.$refs.form1.validate(async (valid) => {
        if (valid) {
          const loading = this.$loading({
            lock: true,
            text: 'Cargando',
            spinner: 'el-icon-loading',
            background: 'rgba(0, 0, 0, 0.3)'
          })
          try {
            this.model.fecha_solicitud = '2020-01-01'
            await this.$http.post(this.url, this.model)
            this.ventanaNuevo = false
            await this.cargarInfo()
          } catch (error) {
            this.$MensajeError(error)
          } finally {
            loading.close()
          }
        }
      })
    },
    async eliminarRegistro(id) {
      const loading = this.$loading({
        lock: true,
        text: 'Cargando',
        spinner: 'el-icon-loading',
        background: 'rgba(0, 0, 0, 0.3)'
      })
      try {
        await this.$http.delete(this.url + '/' + id)
        this.ventanaDetalle = false
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
      try {
        let ret = await this.$http.get(this.url)
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
  margin-bottom: 0.7rem;
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
</style>
