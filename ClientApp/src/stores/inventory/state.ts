import type { Producto } from '../../models/Producto'

export interface InventoryState {
  products: Producto[]
  selectedProduct: Producto | null
  isLoading: boolean
  error: string | null
}

export const state = (): InventoryState => ({
  products: [],
  selectedProduct: null,
  isLoading: false,
  error: null,
})
