export interface User {
	id?: string;
	username?: string;
	email?: string;
	firstName?: string;
	lastName?: string;
	emailVerified?: boolean;
}

// export interface AccessOptions {
//     redirectUri?: string;
// }

// export declare class UseAuth {
//     claim: string[]; 
//     token: TokenParsed;
//     login(options?: AccessOptions): Promise<void>;
//     logout(options?: AccessOptions): Promise<void>;
//     createLoginUrl(options?: AccessOptions): Promise<string>;  
//     createLogoutUrl(options?: AccessOptions): string;
// }