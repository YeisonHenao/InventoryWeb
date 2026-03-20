import type { AuthState } from './state'

export const isAuthenticated = (state: AuthState): boolean => !!state.token
export const userName = (state: AuthState): string => state.user?.nombre ?? ''
