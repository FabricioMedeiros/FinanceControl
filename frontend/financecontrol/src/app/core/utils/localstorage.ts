import { jwtDecode } from "jwt-decode";
import { JwtToken } from "src/app/features/account/models/jtw.token";

export class LocalStorageUtils {

    public getUser() {
        return localStorage.getItem('financecontrol.user');
    }

    public saveLocalUserData(response: any) {
        if (response && response.token) {
            this.saveTokenUser(response.token);
    
            try {
                const decodedToken = jwtDecode<JwtToken>(response.token);
            
                this.saveUser(decodedToken.userName);
                this.saveEmailUser(decodedToken.email);
            } catch (error) {
                console.error('Erro ao decodificar o token:', error);
            }
        } else {
            console.error('Token inválido:', response);
        }
    }    

    public clearLocalUserData() {
        localStorage.removeItem('financecontrol.token');
        localStorage.removeItem('financecontrol.user');
        localStorage.removeItem('financecontrol.email');
    }

    public getTokenUser(): string | null {
        return localStorage.getItem('financecontrol.token');
    }

    public getEmailUser(): string | null {
        return localStorage.getItem('financecontrol.email');
    }

    public saveTokenUser(token: string) {
        localStorage.setItem('financecontrol.token', token);
    }

    public saveUser(user: string) {
        localStorage.setItem('financecontrol.user', user);
    }

    public saveEmailUser(email: string) {
        localStorage.setItem('financecontrol.email', email);
    }
}