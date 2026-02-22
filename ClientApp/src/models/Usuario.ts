import { BaseModel } from "./BaseModel";

export class Usuario extends BaseModel {
    id?: string;
    nombre!: string;
    email!: string;
    password?: string;
    createdAt?: Date;
    updatedAt?: Date;

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

    override toJson(): Record<string, any> {
        return {
            id: this.id,
            nombre: this.nombre,
            email: this.email,
            password: this.password,
            createdAt: this.createdAt?.toISOString(),
            updatedAt: this.updatedAt?.toISOString(),
        };
    }
}