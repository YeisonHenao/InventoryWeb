import axios, { type AxiosInstance, type InternalAxiosRequestConfig } from 'axios';

// Crear instancia de axios con configuración del backend local
const instance: AxiosInstance = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5080',
    timeout: parseInt(import.meta.env.VITE_API_TIMEOUT) || 10000,
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    }
});

// Interceptor para agregar token de autenticación si existe
instance.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        const token = localStorage.getItem('auth_token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        if (import.meta.env.VITE_DEBUG_MODE === 'true') {
            console.log('Request:', config.method?.toUpperCase(), config.url);
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Interceptor para manejar respuestas y errores
instance.interceptors.response.use(
    (response) => {
        if (import.meta.env.VITE_DEBUG_MODE === 'true') {
            console.log('Response:', response.status, response.data);
        }
        return response;
    },
    (error) => {
        if (error.response?.status === 401) {
            // Token expirado o no válido
            localStorage.removeItem('auth_token');
            window.location.href = '/login';
        }
        if (import.meta.env.VITE_DEBUG_MODE === 'true') {
            console.error('Error:', error.response?.status, error.response?.data);
        }
        return Promise.reject(error);
    }
);

export default instance;