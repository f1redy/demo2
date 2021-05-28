<template>
  <div class="mante w-100">
    <el-card>
      <div>
        <h3 class="gris titulo">Especialista</h3>
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
        <el-table-column prop="especialista_id" label="Id" width="100" sortable>
        </el-table-column>
        <el-table-column prop="usuario" label="Usuario" width="150" sortable>
        </el-table-column>
        <el-table-column prop="especialista_desc" label="Nombre" sortable>
        </el-table-column>
        <el-table-column prop="rol_cod" label="Rol" width="100" sortable>
        </el-table-column>
        <el-table-column
          prop="estatus"
          label="Estatus"
          width="100"
          align="center"
          sortable
        >
          <template slot-scope="scope">
            <span :class="scope.row.estatus ? 'verde' : 'rojo'">
              {{ scope.row.estatus ? 'Activo' : 'Inactivo' }}
            </span>
          </template>
        </el-table-column>
        <el-table-column label="Acciones" width="100" align="center">
          <template slot-scope="scope">
            <el-button
              type="text"
              icon="el-icon-view"
              class="gris p-0"
              @click="ver(scope.row, true)"
            ></el-button>

            <el-button
              type="text"
              icon="el-icon-edit"
              class="p-0"
              @click="nuevo(scope.row, false)"
            ></el-button>

            <el-button
              type="text"
              icon="el-icon-delete"
              class="rojo p-0"
              @click="ver(scope.row, false)"
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
          <div>{{ model.especialista_id }}</div>
        </div>
        <div class="row">
          <div>Nombre</div>
          <div>{{ model.especialista_desc }}</div>
        </div>
        <div class="row">
          <div>Usuario</div>
          <div>{{ model.usuario }}</div>
        </div>
        <div class="row">
          <div>Correo</div>
          <div>{{ model.correo }}</div>
        </div>
        <div class="row">
          <div>Rol</div>
          <div>{{ model.rol_cod + ' - ' + model.rol_desc }}</div>
        </div>
        <div class="row">
          <div>Estatus</div>
          <div>{{ model.estatus ? 'Activo' : 'Inactivo' }}</div>
        </div>
      </div>
      <span slot="footer" class="dialog-footer">
        <el-button @click="ventanaDetalle = false">Cancelar</el-button>
        <el-button
          type="danger"
          @click="eliminarRegistro(model.especialista_id)"
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
        label-width="120px"
        size="mini"
      >
        <el-form-item label="Id" prop="especialista_id">
          <el-input
            v-model="model.especialista_id"
            readonly
            :disabled="tipoNuevo"
          ></el-input>
        </el-form-item>
        <el-form-item label="Nombre" prop="especialista_desc">
          <el-input v-model="model.especialista_desc"></el-input>
        </el-form-item>
        <el-form-item label="Usuario" prop="usuario">
          <el-input v-model="model.usuario"></el-input>
        </el-form-item>
        <el-form-item label="Contraseña" prop="contrasena">
          <el-input v-model="model.contrasena" type="password"></el-input>
        </el-form-item>
        <el-form-item label="Correo" prop="correo">
          <el-input v-model="model.correo"></el-input>
        </el-form-item>
        <el-form-item label="Rol" prop="rol_cod">
          <select-search url="api/consultas/roles" v-model="model.rol_cod" />
        </el-form-item>
        <el-form-item label="Estatus" prop="estatus">
          <el-switch v-model="model.estatus"></el-switch>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="ventanaNuevo = false">Cancelar</el-button>
        <el-button type="primary" @click="editarRegistro()" v-if="!tipoNuevo"
          >Editar</el-button
        >
        <el-button type="primary" @click="agregarRegistro()" v-else
          >Agragar</el-button
        >
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
      especialista_id: 0,
      especialista_desc: null,
      usuario: null,
      rol_cod: null,
      rol_desc: null,
      correo: null,
      estatus: true,
      contrasena: null
    },
    ventanaDetalle: false,
    tipoDetalle: true,
    ventanaNuevo: false,
    tipoNuevo: true,
    validaciones: {
      especialista_desc: [requerido],
      usuario: [requerido],
      rol_cod: [requerido],
      correo: [requerido]
    },
    datos: [],
    url: '/api/maestros/especialista'
  }),
  mounted() {
    this.cargarInfo()
  },
  methods: {
    async nuevo(row, val) {
      this.tipoNuevo = val
      this.ventanaNuevo = true
      if (val) {
        this.model.especialista_id = 0
        this.model.especialista_desc = null
        this.model.usuario = null
        this.model.rol_cod = null
        this.model.rol_desc = null
        this.model.correo = null
        this.model.estatus = true
        this.model.contrasena = null
      } else {
        await this.cargarRegistro(row.especialista_id)
      }
    },
    ver(row, val) {
      this.tipoDetalle = val
      this.ventanaDetalle = true
      this.cargarRegistro(row.especialista_id)
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
              this.url + '/' + this.model.especialista_id,
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
      if (!this.model.contrasena && this.tipoNuevo) {
        this.$notify.error('Debe ingresar contraseña')
        return
      }
      this.$refs.form1.validate(async (valid) => {
        if (valid) {
          const loading = this.$loading({
            lock: true,
            text: 'Cargando',
            spinner: 'el-icon-loading',
            background: 'rgba(0, 0, 0, 0.3)'
          })
          try {
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
    width: 120px;
    color: rgb(124, 124, 124);
  }
  div:last-child {
    position: relative;
    color: rgb(105, 103, 103);
    font-weight: 600;
  }
}
</style>
