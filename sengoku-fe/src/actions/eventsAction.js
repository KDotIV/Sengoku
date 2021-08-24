import axios from 'axios';
import { eventsURL } from '../api';

//Action Creator
export const loadEvents = () => async (dispatch) =>{
    //FETCH AXIOS
    const eventsData = await axios.get(eventsURL() + 'GetEvents')
    dispatch({
        type: "FETCH_EVENTS",
        payload: {
            events: eventsData.data,
        },
    })
}