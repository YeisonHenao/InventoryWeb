<template>
  <div>
    <h1>Home Page</h1>
    <p>{{ message }}</p>
  </div>
</template>

<script lang="ts">
import { useHome } from './composable/useHome'

export default {
  name: 'Home',
  setup() {
    const {message} = useHome()

    return {
      message
    }
  }
}
</script>