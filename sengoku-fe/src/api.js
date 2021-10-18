//Base URL
const base_url = 'https://localhost:5001/api/';

const getCurrentMonth = () => {
    const month = new Date().getMonth() + 1;
    if(month < 10)
    {
        return `0${month}`;
    } else {
        return month;
    }
}

const getCurrentDay = () => {
    const day = new Date().getDate() + 1;
    if(day < 10)
    {
        return `0${day}`;
    } else {
        return day;
    }
}

const currentYear = new Date().getFullYear();
const currentMonth = getCurrentMonth();
const currentDay = getCurrentDay();
const currentDate = `${currentYear}-${currentMonth}-${currentDay}`;

console.log(currentDate);

//Events
const eventsApi = `events/`;
//Orders
const ordersApi = `orders/`;
//Products
const productsApi = `products/`;
//Users
const usersApi = `users/`;
//Players
const playersApi = `playercards/`;

export const eventsURL = () => `${base_url}${eventsApi}`;
export const ordersURL = () => `${base_url}${ordersApi}`;
export const productsURL = () => `${base_url}${productsApi}`;
export const userURL = () => `${base_url}${usersApi}`;
export const playerURL = () => `${base_url}${playersApi}`;