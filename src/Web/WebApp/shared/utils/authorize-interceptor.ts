import axios from "axios";

const BASE_HOST = "http:/localhost:5000";
const token = localStorage.getItem("token");

axios.interceptors.request.use(
    config => {
        config.baseURL = BASE_HOST;
        if(token) {
            config.headers["Authorization"] = "Bearer " + token;
        }
        
        return config;
    },
    error => {
        console.log("Request error: ", error);
        return Promise.reject(error);
    }
)