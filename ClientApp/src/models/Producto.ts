import { BaseModel } from './BaseModel';

/**
 * Modelo de Producto
 * Extiende BaseModel para decodificación y serialización automática
 */
export class Producto extends BaseModel {
    id?: string;
    nombre!: string;
    descripcion?: string;
    precio!: number;
    cantidad!: number;
    createdAt?: Date;
    updatedAt?: Date;

    /**
     * Decodifica datos JSON del servidor al modelo
     * Puede sobrescribir si necesitas transformaciones específicas
     */
    override fromJson(json: any): this {
        super.fromJson(json);
        
        // Transformaciones específicas si es necesario
        if (json.createdAt) {
            this.createdAt = new Date(json.createdAt);
        }
        if (json.updatedAt) {
            this.updatedAt = new Date(json.updatedAt);
        }

        return this;
    }

    /**
     * Serializa el modelo a JSON para enviar al servidor
     */
    override toJson(): Record<string, any> {
        return {
            id: this.id,
            nombre: this.nombre,
            descripcion: this.descripcion,
            precio: this.precio,
            cantidad: this.cantidad,
            createdAt: this.createdAt?.toISOString(),
            updatedAt: this.updatedAt?.toISOString(),
        };
    }
}
