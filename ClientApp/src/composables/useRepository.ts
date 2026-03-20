import { ref } from 'vue';
import type { BaseRepository } from '../repositories/BaseRepository';
import type { BaseModel } from '../models/BaseModel';
import { AxiosError } from 'axios';

/**
 * Composable genérico para usar repositorios
 * Maneja loading, error y data de forma automática
 * @param _repository - Instancia del repositorio a usar
 */
export function useRepository<T extends BaseModel>(_repository: BaseRepository<T>) {
    const data = ref<T | null>(null);
    const dataList = ref<T[]>([]);
    const loading = ref(false);
    const error = ref<AxiosError | null>(null);

    const execute = async <R>(fn: () => Promise<R>): Promise<R | null> => {
        loading.value = true;
        error.value = null;
        try {
            const result = await fn();
            return result;
        } catch (err) {
            error.value = err as AxiosError;
            console.error('Repository error:', err);
            throw err;
        } finally {
            loading.value = false;
        }
    };

    const executeList = async <R extends T[]>(fn: () => Promise<R>): Promise<R | null> => {
        loading.value = true;
        error.value = null;
        try {
            const result = await fn();
            dataList.value = result;
            return result;
        } catch (err) {
            error.value = err as AxiosError;
            console.error('Repository error:', err);
            throw err;
        } finally {
            loading.value = false;
        }
    };

    const executeSingle = async <R extends T>(fn: () => Promise<R>): Promise<R | null> => {
        loading.value = true;
        error.value = null;
        try {
            const result = await fn();
            data.value = result;
            return result;
        } catch (err) {
            error.value = err as AxiosError;
            console.error('Repository error:', err);
            throw err;
        } finally {
            loading.value = false;
        }
    };

    return {
        data,
        dataList,
        loading,
        error,
        execute,
        executeList,
        executeSingle,
    };
}
