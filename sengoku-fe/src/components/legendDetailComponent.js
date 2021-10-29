import React, { useEffect, useCallback, useRef } from 'react';
import { useDispatch, useSelector } from "react-redux";
import { useLocation } from 'react-router';
//Styling and Animation
import styled from 'styled-components';
import { motion } from "framer-motion";
import test from '../media/ZyoDzL3.jpg';
//Redux & Components
import { useHistory } from 'react-router-dom';
import { getLegend } from '../actions/legendsAction';
import YoutubeEmbed from './youtubeComponent';

const LegendDetail = () => {
    const history = useHistory();
    const location = useLocation();
    const pathId = location.pathname.split("/")[2];
    const dispatch = useDispatch();

    //GetLegend on Refresh
    const isInitialState = useRef(true);
    const reFetchLegend = useCallback (() => {
        dispatch(getLegend(pathId))
    }, [dispatch])

    useEffect(() => reFetchLegend(), [dispatch])
    //Exit Details
    const exitDetailHandler = (e) => {
        const element = e.target;
        if(element.classList.contains('shadow')){
            document.body.style.overflow = 'auto';
            history.push("/legends/");
        }
    }
    const {legend, isLoading} = useSelector((state) => state.legends)
    return (
        <>
        {!isLoading && (
            <CardShadow className="shadow" onClick={exitDetailHandler}>
                <Detail>
                    <Summary>
                        <h2>{legend.subject}</h2>
                        <h3>{legend.summary}</h3>
                        <h3>{legend.game}</h3>
                    </Summary>
                    <PlotPoints>
                        <h3>PlotPoints</h3>
                        {legend.plotPoints.map((data) =>(
                            <>
                            <p key={data.plotId}>{data.text}</p>
                            {data.image && <img key={data.plotId} src="../media/ZyoDzL3.jpg" alt="images" />}
                            {data.clipRef && <YoutubeEmbed embedId={data.clipRef} />}
                            </>
                        ))}
                        </PlotPoints>
                </Detail>
            </CardShadow>
        )}
        </>
    );
};

const CardShadow = styled(motion.div)`
    width: 100%;
    min-height: 100vh;
    overflow-y: scroll;
    background: rgba(0,0,0,0.5);
    position: fixed;
    top: 0;
    left: 0;
`;

const Detail = styled(motion.div)`
    width: 80%;
    border-radius: 1rem;
    padding: 2rem 20rem;
    margin-top: 12rem;
    background: #1b213a;
    position: absolute;
    left: 10%;
    color: white;
    img{
        justify-content: center;
        width: 80%;
    }
`;

const Summary = styled(motion.div)`
    display: grid;
    grid-auto-flow: row;
    align-items: center;
    text-align: center;
    padding: 5rem 10rem;
`;

const PlotPoints = styled(motion.div)`
    display: grid;
    grid-auto-flow: row;
    align-items: center;
    text-align: center;
    
    p{
        padding: 3rem;
    }
`;

export default LegendDetail;