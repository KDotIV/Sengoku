import axios from 'axios';
import { eventsURL } from '../api';

//Action Creator
export const loadEvents = () => async (dispatch) =>{
    const eventsData = await axios.get(eventsURL() + 'GetEvents')
    dispatch({
        type: "FETCH_EVENTS",
        payload: {
            eventResult: eventsData.data,
        },
    })
}

export const getEvent = (eventId) => async (dispatch) =>{
    const eventData = await axios.get(`${eventsURL()}GetEvent/id/${eventId}`);

    dispatch({
        type: "GET_EVENT",
        payload: {
            eventResult: eventData.data,
        },
    });
}