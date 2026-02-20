<template>
  <div>
    <h1 class="login-title">Login</h1>
    <form>
      <div>
        <label for="username">Username:</label>
        <input id="username" v-model="username" type="text" required />
      </div>
      <div>
        <label for="password">Password:</label>
        <input id="password" v-model="password" type="password" required />
      </div>
      <button type="submit" @click.prevent="login">Login</button>
    </form>
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