import axios from 'axios';
import { playerURL } from '../api';

//Action Creator
export const loadPlayers = () => async (dispatch) =>{
    //FETCH AXIOS
    const playersData = await axios.get(playerURL() + 'GetAllPlayers')
    dispatch({
        type: "FETCH_PLAYERS",
        payload: {
            playerResult: playersData.data,
        },
    })
}