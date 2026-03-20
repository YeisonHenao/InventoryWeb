import type { InventoryState } from './state'

export const productsCount = (state: InventoryState): number => state.products.length
export const hasProducts = (state: InventoryState): boolean => state.products.length > 0
