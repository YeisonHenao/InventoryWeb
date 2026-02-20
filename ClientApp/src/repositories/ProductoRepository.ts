import { BaseRepository } from './BaseRepository';
import { Producto } from '../models/Producto';
import type { AxiosInstance } from 'axios';

/**
 * Repositorio para manejar operaciones CRUD de Productos
 * Recibe una instancia de axios en el constructor
 */
export class ProductoRepository extends BaseRepository<Producto> {
    constructor(instance: AxiosInstance) {
        super(instance, Producto as any);
    }

    /**
     * Obtener todos los productos
     */
    async obtenerTodos(): Promise<Producto[]> {
        return this.getList('/api/productos');
    }

    /**
     * Obtener un producto por ID
     */
    async obtenerPorId(id: string): Promise<Producto> {
        return this.get(`/api/productos/${id}`);
    }

    /**
     * Crear un nuevo producto
     */
    async crear(producto: Producto | Record<string, any>): Promise<Producto> {
        return this.post('/api/productos', producto);
    }

    /**
     * Actualizar un producto
     */
    async actualizar(id: string, producto: Partial<Producto> | Record<string, any>): Promise<Producto> {
        return this.put(`/api/productos/${id}`, producto);
    }

    /**
     * Eliminar un producto
     */
    async eliminar(id: string): Promise<any> {
        return this.delete(`/api/productos/${id}`);
    }

    /**
     * Buscar productos por nombre
     */
    async buscar(nombre: string): Promise<Producto[]> {
        return this.getList('/api/productos/buscar', { nombre });
    }
}
