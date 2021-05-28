<template>
  <el-menu class="h-100" router>
    <el-submenu index="1">
      <template slot="title">
        <i class="el-icon-s-management"></i>
        <span class="color-b">Maestros</span>
      </template>
      <el-menu-item index="/maestro/especialista">Especialistas</el-menu-item>
      <el-menu-item index="/maestro/tarea">Tareas</el-menu-item>
      <el-menu-item index="/maestro/solicitante">Solicitantes</el-menu-item>
      <el-menu-item index="/maestro/area">Areas</el-menu-item>
    </el-submenu>
    <el-submenu index="2">
      <template slot="title">
        <i class="el-icon-message-solid"></i>
        <span class="color-b">Gestiones</span>
      </template>
      <el-menu-item index="/gestion/asignacion">Asignacion Tarea</el-menu-item>
      <el-menu-item index="/gestion/alertas">Alertas</el-menu-item>
      <el-menu-item index="/gestion/exportar">Exportar</el-menu-item>
      <el-menu-item index="/gestion/Modificacion">Modificaciones </el-menu-item>
    </el-submenu>

    <el-submenu index="3">
      <template slot="title">
        <i class="el-icon-s-marketing"></i>
        <span class="color-b"> Reportes</span>
      </template>
      <el-menu-item index="/reportes/por-criticidad"
        >Por Criticidad</el-menu-item
      >
      <el-menu-item index="/reportes/en-tiempo">En Tiempo</el-menu-item>
      <el-menu-item index="/reportes/fuera-de-tiempo"
        >Fuera de Tiempo</el-menu-item
      >
      <el-menu-item index="/reportes/por-responsable"
        >Por Responsable
      </el-menu-item>
      <el-menu-item index="/reportes/por-area">Por Area</el-menu-item>
    </el-submenu>
  </el-menu>
</template>

<script>
export default {}
</script>
