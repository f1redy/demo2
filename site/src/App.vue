<template>
  <div id="app">
    <router-view />
  </div>
</template>

<style>
html,
body,
#app {
  height: 100%;
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  background: #f3faff;
}

.h-100 {
  height: 100%;
}
.w-100 {
  width: 100%;
}

.color-b {
  color: #6b6c70;
  font-weight: 600;
}
.p-0 {
  padding: 0 !important;
}
.float-left {
  float: left;
}
.float-right {
  float: right;
}
.mt-2 {
  margin-top: 1rem;
}
.mt-1 {
  margin-top: 0.5rem;
}
</style>
