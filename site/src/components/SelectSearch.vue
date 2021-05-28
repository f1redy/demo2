<template>
  <el-select
    ref="main"
    :value="registro"
    filterable
    @input="changeValue"
    @change="change"
    @blur="handleBlur"
    value-key="id"
    :readonly="readonly"
    :disabled="readonly"
    v-loading="cargando"
  >
    <el-option
      v-for="x in info"
      :key="x.id"
      :label="x.etiqueta"
      :value="x.id"
    />
  </el-select>
</template>

<script>
export default {
  data: () => ({
    info: [],
    cargando: false,
    registro: null
  }),
  props: ['url', 'value', 'readonly'],

  async mounted() {
    await this.cargarInfo()
    if (
      this.info &&
      Array.isArray(this.info) &&
      this.info.find((x) => x.id === this.value)
    ) {
      this.registro = this.value
    } else {
      this.$emit('input', null)
      this.$emit('actualizar', null)
    }
  },
  methods: {
    changeValue(val) {
      this.$emit('input', val)
    },
    change(val) {
      this.$emit('change', val)
    },
    handleBlur(event) {
      this.$emit('blur', event)
    },
    focus() {
      this.$refs.main.focus()
    },
    async cargarInfo() {
      try {
        this.cargando = true
        const info = await this.$http.get(this.url)
        this.info = info.data
        this.cargando = false
      } catch (error) {
        this.$msgError(error)
      }
    }
  },
  watch: {
    value(e) {
      this.registro = e
    }
  }
}
</script>

<style></style>
