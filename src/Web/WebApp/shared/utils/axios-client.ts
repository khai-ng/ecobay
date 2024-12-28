import axios from "axios";

const BASE_HOST = "http://localhost:5100/";


const httpClient = axios.create({
    baseURL: BASE_HOST,
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

export default httpClient;