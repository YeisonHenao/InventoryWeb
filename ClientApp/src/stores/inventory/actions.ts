import { Producto } from '../../models/Producto'

export async function fetchProducts(this: any) {
  this.isLoading = true
  this.error = null

  try {
    // TODO: reemplazar por llamada real al API
    await new Promise((resolve) => setTimeout(resolve, 500))

    this.products = [
      new Producto().fromJson({
        id: '1',
        nombre: 'Producto de ejemplo',
        descripcion: 'Descripción de ejemplo',
        precio: 99.9,
        cantidad: 10,
      }),
    ]
  } catch (error) {
    this.error = (error as Error).message || 'Error al cargar productos'
  } finally {
    this.isLoading = false
  }
}

export function selectProduct(this: any, product: Producto | null) {
  this.selectedProduct = product
}

export function addProduct(this: any, product: Producto) {
  this.products = [...this.products, product]
}

export function updateProduct(this: any, product: Producto) {
  this.products = this.products.map((p: Producto) => (p.id === product.id ? product : p))
}

export function removeProduct(this: any, productId: string) {
  this.products = this.products.filter((p: Producto) => p.id !== productId)
}
