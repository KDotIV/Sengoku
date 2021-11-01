import axios from "axios";
import { legendsURL } from "../api";

//Action Center
export const loadLegends = () => async (dispatch) =>{
    dispatch({
        type: "LOADING_LEGENDS",
    });

    const legendsData = await axios.get(legendsURL() + 'GetAllLegends')    
    dispatch({
        type: "FETCH_LEGENDS",
        payload: {
            legendResult: legendsData.data,
        },
    })
}

export const getLegend = (legendId) => async (dispatch, getState) => {

    const { dataPresent } = getState().legends;

    const legendData = await axios.get(`${legendsURL()}GetLegend/id/${legendId}`);

    if(dataPresent) {
        return null;
    } else {
        return (
            dispatch({
                type: "GET_LEGEND",
                payload: {
                    legendResult: legendData.data,
                },
            })
        );
    }
}