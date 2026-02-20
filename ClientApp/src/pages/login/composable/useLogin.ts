import { ref } from 'vue';

export const useLogin = () => {

  const username = ref('');
  const password = ref('');

  const login = () => {
    // Aquí puedes agregar la lógica de autenticación, como llamar a una API para verificar las credenciales
    console.log('Username:', username.value);
    console.log('Password:', password.value);
  };

  return {
    username,
    password,
    login
  };
}
