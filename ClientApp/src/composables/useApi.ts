import { ref } from 'vue';
import instance from '../config/instanciaLocal';
import { AxiosError, type AxiosResponse } from 'axios';

export function useApi<T = any>() {
    const data = ref<T | null>(null);
    const loading = ref(false);
    const error = ref<AxiosError | null>(null);

    const get = async (url: string, params?: Record<string, any>) => {
        loading.value = true;
        error.value = null;
        try {
            const response: AxiosResponse<T> = await instance.get(url, { params });
            data.value = response.data;
            return response.data;
        } catch (err) {
            error.value = err as AxiosError;
            throw err;
        } finally {
            loading.value = false;
        }
    };

    const post = async (url: string, payload?: any) => {
        loading.value = true;
        error.value = null;
        try {
            const response: AxiosResponse<T> = await instance.post(url, payload);
            data.value = response.data;
            return response.data;
        } catch (err) {
            error.value = err as AxiosError;
            throw err;
        } finally {
            loading.value = false;
        }
    };

    const put = async (url: string, payload?: any) => {
        loading.value = true;
        error.value = null;
        try {
            const response: AxiosResponse<T> = await instance.put(url, payload);
            data.value = response.data;
            return response.data;
        } catch (err) {
            error.value = err as AxiosError;
            throw err;
        } finally {
            loading.value = false;
        }
    };

    const delete_ = async (url: string) => {
        loading.value = true;
        error.value = null;
        try {
            const response: AxiosResponse<T> = await instance.delete(url);
            data.value = response.data;
            return response.data;
        } catch (err) {
            error.value = err as AxiosError;
            throw err;
        } finally {
            loading.value = false;
        }
    };

    return {
        data,
        loading,
        error,
        get,
        post,
        put,
        delete: delete_
    };
}
