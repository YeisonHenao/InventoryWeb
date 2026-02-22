import { BaseRepository } from "./BaseRepository";
import { Usuario } from "../models/Usuario";
import type { AxiosInstance } from "axios";

export class AuthRepository extends BaseRepository<Usuario> {
    constructor(instance: AxiosInstance) {
        super(instance, Usuario as any);
    }


    async loginAsync(email: string, password: string): Promise<Usuario> {
        return this.post("/api/auth/login", { email, password });
    }

    async registerAsync(usuario: Usuario | Record<string, any>): Promise<Usuario> {
        return this.post("/api/auth/register", usuario);
    }
}