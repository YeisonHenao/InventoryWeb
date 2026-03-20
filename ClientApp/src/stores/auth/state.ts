import type { Usuario } from '../../models/Usuario'

export interface AuthState {
  user: Usuario | null
  token: string | null
  isLoading: boolean
  error: string | null
}

export const state = (): AuthState => ({
  user: null,
  token: null,
  isLoading: false,
  error: null,
})
