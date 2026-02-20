/**
 * Clase base para all modelos de la aplicación
 * Proporciona métodos estáticos para decodificar y serializar datos
 */
export abstract class BaseModel {
    /**
     * Método estático para decodificar/transformar datos del servidor
     * @param data - Datos crudos del servidor
     * @returns Instancia de la clase transformada
     */
    static decode<T extends BaseModel>(data: any): T {
        const instance = new (this as any)();
        return instance.fromJson(data);
    }

    /**
     * Método estático para decodificar múltiples registros
     * @param dataArray - Array de datos crudos del servidor
     * @returns Array de instancias transformadas
     */
    static decodeArray<T extends BaseModel>(dataArray: any[]): T[] {
        return dataArray.map((item) => this.decode<T>(item));
    }

    /**
     * Método de instancia para transformar datos JSON al modelo
     * Debe ser sobrescrito en las clases hijas
     * @param json - Datos JSON del servidor
     * @returns this para encadenamiento
     */
    fromJson(json: any): this {
        Object.assign(this, json);
        return this;
    }

    /**
     * Método de instancia para serializar el modelo a JSON
     * Debe ser sobrescrito en las clases hijas si se necesita transformación
     * @returns Objeto JSON para enviar al servidor
     */
    toJson(): Record<string, any> {
        const json: Record<string, any> = {};
        for (const key in this) {
            if (this.hasOwnProperty(key) && typeof this[key] !== 'function') {
                json[key] = this[key];
            }
        }
        return json;
    }

    /**
     * Método para obtener una copia del modelo
     * @returns Nueva instancia con los mismos datos
     */
    clone<T extends BaseModel>(): T {
        return Object.assign(Object.create(Object.getPrototypeOf(this)), this) as T;
    }
}
