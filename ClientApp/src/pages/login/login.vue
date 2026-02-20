<template>
  <div class="login-container">
    <div class="login-card">
      <h1 class="login-title">Bienvenido</h1>
      <p class="login-subtitle">Ingresa tus credenciales para continuar</p>
      
      <form class="login-form" @submit.prevent="login">
        <div class="input-group">
          <label for="username">Usuario</label>
          <input 
            id="username" 
            v-model="username" 
            type="text" 
            placeholder="Usuario"
            required 
          />
        </div>

        <div class="input-group">
          <label for="password">Contraseña</label>
          <input 
            id="password" 
            v-model="password" 
            type="password" 
            placeholder="••••••••"
            required 
          />
        </div>

        <button type="submit" class="login-button">
          Iniciar Sesión
        </button>
      </form>
    </div>
  </div>
</template>

<script lang="ts">

import { useLogin } from './composable/useLogin';

export default {
  name: 'Login',
  setup() {
    const { username, password, login } = useLogin();

    return {
      username,
      password,
      login
    };
  }
};

</script>

<style>
@import '../../assets/styles/views/Login/login.css';
</style>