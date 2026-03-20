import { Usuario } from '../../models/Usuario'

export type LoginPayload = {
  email: string
  password: string
}

export async function login(this: any, payload: LoginPayload) {
  // Ejemplo básico para demostración; reemplaza con llamada real al backend
  this.isLoading = true
  this.error = null

  try {
    await new Promise((resolve) => setTimeout(resolve, 500))

    const user = new Usuario().fromJson({
      id: 'demo',
      nombre: 'Demo Usuario',
      email: payload.email,
    })

    this.user = user
    this.token = 'token-de-prueba'
  } catch (error) {
    this.error = (error as Error).message || 'Error al iniciar sesión'
  } finally {
    this.isLoading = false
  }
}

export function logout(this: any) {
  this.user = null
  this.token = null
}

export function setToken(this: any, token: string | null) {
  this.token = token
}

export function setUser(this: any, user: Usuario | null) {
  this.user = user
}
