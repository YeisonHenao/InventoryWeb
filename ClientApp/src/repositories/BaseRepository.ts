import type { AxiosInstance, AxiosResponse } from 'axios';
import type { BaseModel } from '../models/BaseModel';

type ModelConstructor<T extends BaseModel> = typeof BaseModel & {
    new (): T;
    decode<U extends BaseModel>(data: any): U;
    decodeArray<U extends BaseModel>(dataArray: any[]): U[];
};

/**
 * Repositorio base para manejar operaciones CRUD
 * Recibe una instancia de axios para todas las peticiones
 */
export abstract class BaseRepository<T extends BaseModel> {
    protected instance: AxiosInstance;
    protected model: ModelConstructor<T>;

    constructor(instance: AxiosInstance, model: ModelConstructor<T>) {
        this.instance = instance;
        this.model = model;
    }

    /**
     * GET - Obtener un recurso
     * @param url - URL del endpoint
     * @param params - Parámetros de query opcionales
     * @returns Instancia del modelo decodificada
     */
    async get(url: string, params?: Record<string, any>): Promise<T> {
        const response: AxiosResponse = await this.instance.get(url, { params });
        return (this.model.decode as any)(response.data) as T;
    }

    /**
     * GET - Obtener múltiples recursos
     * @param url - URL del endpoint
     * @param params - Parámetros de query opcionales
     * @returns Array de instancias del modelo decodificadas
     */
    async getList(url: string, params?: Record<string, any>): Promise<T[]> {
        const response: AxiosResponse = await this.instance.get(url, { params });
        const dataArray = Array.isArray(response.data) ? response.data : response.data.data || [];
        return (this.model.decodeArray as any)(dataArray) as T[];
    }

    /**
     * POST - Crear un nuevo recurso
     * @param url - URL del endpoint
     * @param payload - Datos a enviar (puede ser instancia del modelo o objeto plano)
     * @returns Instancia del modelo decodificada con la respuesta del servidor
     */
    async post(url: string, payload: T | Record<string, any>): Promise<T> {
        const data = payload instanceof this.model ? payload.toJson() : payload;
        const response: AxiosResponse = await this.instance.post(url, data);
        return (this.model.decode as any)(response.data) as T;
    }

    /**
     * PUT - Actualizar un recurso
     * @param url - URL del endpoint
     * @param payload - Datos a enviar (puede ser instancia del modelo o objeto plano)
     * @returns Instancia del modelo decodificada con la respuesta del servidor
     */
    async put(url: string, payload: T | Record<string, any>): Promise<T> {
        const data = payload instanceof this.model ? payload.toJson() : payload;
        const response: AxiosResponse = await this.instance.put(url, data);
        return (this.model.decode as any)(response.data) as T;
    }

    /**
     * PATCH - Actualizar parcialmente un recurso
     * @param url - URL del endpoint
     * @param payload - Datos a enviar (puede ser instancia del modelo o objeto plano)
     * @returns Instancia del modelo decodificada con la respuesta del servidor
     */
    async patch(url: string, payload: Partial<T> | Record<string, any>): Promise<T> {
        const data = payload instanceof this.model ? payload.toJson() : payload;
        const response: AxiosResponse = await this.instance.patch(url, data);
        return (this.model.decode as any)(response.data) as T;
    }

    /**
     * DELETE - Eliminar un recurso
     * @param url - URL del endpoint
     * @returns Respuesta del servidor
     */
    async delete(url: string): Promise<any> {
        const response: AxiosResponse = await this.instance.delete(url);
        return response.data;
    }
}
