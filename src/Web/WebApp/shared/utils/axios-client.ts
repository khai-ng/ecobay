import axios, { AxiosResponse } from "axios";
import { AppResult } from "./api.model";

if (!process.env.NEXT_PUBLIC_API_URL) {
    throw new Error("API URL is not defined");
}

const httpClient = axios.create({
    baseURL: process.env.NEXT_PUBLIC_API_URL,
    withCredentials : false,
    timeout: 10_000
})

httpClient.interceptors.request.use(
    config => {
        const token = localStorage.getItem("token");
        if(token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        
        return config;
    },
    error => {
        console.log("Request error: ", error);
        return Promise.reject(error);
    }
)

// httpClient.interceptors.response.use(
//     response => {
//         if(response.status >= 200 && response.status < 300) {
//             return response;
//         }
//         return Promise.reject(response);
//     },
//     error => {
//         console.log("Response error: ", error);
//         const { response } = error;
//         if (response && response.status === 401) {
//             // Handle unauthorized access
//             localStorage.removeItem("token");
//             window.location.href = "/login";
//         }
//         return Promise.reject(response);
//     }
// )

export function AppResultFrom<T>(result: AxiosResponse) {
    return new AppResult<T>({
        status: result.status,
        data: result.data?.data,
        message: result.data?.data?.message || null,
        errors: result.data?.data?.errors || null
    });
}

export default httpClient;