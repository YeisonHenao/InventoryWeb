import { ref } from 'vue';

export const useHome = () => {
  const message = ref('Welcome to the Home Page!');

  return {
    message,
  };
}