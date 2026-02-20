# Sistema de Repositorios con Modelos Tipados

## 🎯 Descripción

Sistema completo de gestión de datos con tipado fuerte, decodificación automática y serialización de modelos para Vue 3 + TypeScript.

## 📁 Estructura

```
src/
├── models/
│   ├── BaseModel.ts          # Clase base para todos los modelos
│   ├── Producto.ts           # Ejemplo de modelo
│   └── [Otros modelos...]
├── repositories/
│   ├── BaseRepository.ts      # Clase base para repositorios
│   ├── ProductoRepository.ts  # Ejemplo de repositorio
│   ├── EJEMPLOS.ts            # Ejemplos de uso
│   └── [Otros repositorios...]
├── composables/
│   ├── useApi.ts              # Composable genérico para axios
│   ├── useRepository.ts       # Composable para repositorios
│   └── [Otros composables...]
└── config/
    └── instanciaLocal.ts      # Configuración de axios
```

## 🚀 Uso rápido

### 1. Crear un modelo

```typescript
import { BaseModel } from '@/models/BaseModel';

export class Producto extends BaseModel {
    id?: string;
    nombre!: string;
    precio!: number;

    override fromJson(json: any): this {
        super.fromJson(json);
        // Transformaciones específicas
        return this;
    }

    override toJson(): Record<string, any> {
        return {
            id: this.id,
            nombre: this.nombre,
            precio: this.precio,
        };
    }
}
```

### 2. Crear un repositorio

```typescript
import { BaseRepository } from '@/repositories/BaseRepository';
import { AxiosInstance } from 'axios';
import { Producto } from '@/models/Producto';

export class ProductoRepository extends BaseRepository<Producto> {
    constructor(instance: AxiosInstance) {
        super(instance, Producto as any);
    }

    async obtenerTodos(): Promise<Producto[]> {
        return this.getList('/api/productos');
    }

    async crear(producto: Producto): Promise<Producto> {
        return this.post('/api/productos', producto);
    }
}
```

### 3. Usar en un componente

```typescript
import { useRepository } from '@/composables/useRepository';
import { ProductoRepository } from '@/repositories/ProductoRepository';
import instance from '@/config/instanciaLocal';

export default {
    setup() {
        const repo = new ProductoRepository(instance);
        const { dataList, loading, error, executeList } = useRepository(repo);

        const cargar = async () => {
            await executeList(() => repo.obtenerTodos());
        };

        return { dataList, loading, error, cargar };
    }
};
```

## 📚 API

### BaseModel

#### Métodos estáticos

- `decode<T>(data: any): T` - Decodifica un objeto a instancia del modelo
- `decodeArray<T>(data: any[]): T[]` - Decodifica un array de objetos

#### Métodos de instancia

- `fromJson(json: any): this` - Transforma JSON en propiedades del modelo
- `toJson(): Record<string, any>` - Serializa el modelo a objeto plano
- `clone<T>(): T` - Crea una copia del modelo

### BaseRepository<T>

Métodos disponibles:

- `get(url: string, params?): Promise<T>` - GET de un recurso
- `getList(url: string, params?): Promise<T[]>` - GET de múltiples recursos
- `post(url: string, payload: T | object): Promise<T>` - POST
- `put(url: string, payload: T | object): Promise<T>` - PUT
- `patch(url: string, payload: Partial<T> | object): Promise<T>` - PATCH
- `delete(url: string): Promise<any>` - DELETE

### useRepository<T>

```typescript
const {
    data,              // ref<T | null> - Un solo recurso
    dataList,          // ref<T[]> - Lista de recursos
    loading,           // ref<boolean> - Estado de carga
    error,             // ref<AxiosError | null> - Error si ocurre
    execute,           // (fn) => Promise - Ejecutor genérico
    executeList,       // (fn) => Promise - Ejecutor para listas
    executeSingle      // (fn) => Promise - Ejecutor para un recurso
} = useRepository(repository);
```

## ⚙️ Configuración

### Variables de entorno (.env.local)

```
VITE_API_BASE_URL=http://localhost:5080
VITE_API_TIMEOUT=10000
VITE_DEBUG_MODE=true
```

### Instancia de axios (src/config/instanciaLocal.ts)

Por defecto usa:
- BaseURL desde `VITE_API_BASE_URL`
- Timeout desde `VITE_API_TIMEOUT`
- Interceptadores para auth (Bearer token) y debug

## 🔐 Manejo de autenticación

El sistema guarda el token en `localStorage.auth_token`:

```typescript
// Guardar token
localStorage.setItem('auth_token', token);

// El interceptador automaticamente lo agregará a cada petición
// Authorization: Bearer <token>
```

Si el servidor retorna 401, se limpia el token y redirige a `/login`.

## 🛠️ Crear instancia personalizada

```typescript
import axios from 'axios';
import { ProductoRepository } from '@/repositories/ProductoRepository';

// Crear instancia con configuración personalizada
const customInstance = axios.create({
    baseURL: 'https://api.example.com',
    timeout: 30000,
    headers: {
        'X-Custom-Header': 'value'
    }
});

// Usar con repositorio
const repo = new ProductoRepository(customInstance);
```

## 💡 Ejemplo completo

Ver [EJEMPLOS.ts](./EJEMPLOS.ts) para ejemplos detallados.

## 📝 Notas

- Los modelos manejan la transformación automática de datos JSON
- Las respuestas del servidor pueden incluir `status`, `message` y `success` en el nivel raíz
- El manejo de errores se hace con try/catch en los componentes
- Todos los métodos del repositorio retornan instancias tipadas automáticamente
