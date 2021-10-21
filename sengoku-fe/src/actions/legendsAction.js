import axios from "axios";
import { legendsURL } from "../api";

//Action Center
export const loadLegends = () => async (dispatch) =>{
    const legendsData = await axios.get(legendsURL() + 'GetAllLegends')
    dispatch({
        type: "FETCH_LEGENDS",
        payload: {
            legendResult: legendsData.data,
        },
    })
}

export const getLegend = (legendId) => async (dispatch) =>{
    const legendData = await axios.get(`${legendsURL()}GetLegend/id/${legendId}`);

    dispatch({
        type: "GET_LEGEND",
        payload: {
            legendResult: legendData.data,
        },
    });
}