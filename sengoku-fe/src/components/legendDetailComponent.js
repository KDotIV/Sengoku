import React, { useEffect, useCallback, useState } from 'react';
import { useDispatch, useSelector } from "react-redux";
import { useLocation } from 'react-router';
//Styling and Animation
import styled from 'styled-components';
import { motion } from "framer-motion";
//Redux & Components
import { useHistory } from 'react-router-dom';
import { getLegend } from '../actions/legendsAction';
import { saveState, loadState } from '../store';
import YoutubeEmbed from './youtubeComponent';
import PlotComponenet from './plotPointsComponent';

const LegendDetail = () => {
    const history = useHistory();
    const location = useLocation();
    const pathId = location.pathname.split("/")[2];
    const dispatch = useDispatch();
    const {legend, isLoading} = useSelector((state) => state.legends)

    //GetLegend on Refresh
    const reFetchLegend = useCallback(() => {
        dispatch(getLegend(pathId))
    }, [dispatch])
    
    useEffect(() => reFetchLegend(), []);

    saveState(legend);
    //Exit Details
    const exitDetailHandler = (e) => {
        const element = e.target;
        if(element.classList.contains('shadow')){
            document.body.style.overflow = 'auto';
            history.push("/legends/");
        }
    }
    return (
        <>
        {!isLoading && (
            <CardShadow className="shadow" onClick={exitDetailHandler}>
                <Detail>
                    <h2>{legend.subject}</h2>
                    <Summary>
                        {legend.mainClip && <YoutubeEmbed embedId={legend.mainClip}/>}
                        <h3>{legend.summary}</h3>
                        <h3>{legend.game}</h3>
                    </Summary>
                    <h2>PlotPoints</h2>
                    <PlotPoints>
                    {legend.plotPoints.map((result) => (
                        <PlotComponenet
                        id={result.legendId}
                        title={result.title}
                        text={result.text}
                        image={result.image}
                        key={result.legendId}
                        />
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
    padding: 2rem;
    margin-top: 12rem;
    background: #1b213a;
    position: absolute;
    left: 10%;
    color: white;
    img{
        justify-content: center;
        width: 80%;
    }
    h2{
        text-align: center;
        padding-bottom: 2rem;
    }
    h3{
        text-align: center;
    }
`;
const Summary = styled(motion.div)`
    display: grid;
    grid-template-columns: 1fr 1fr;
    grid-column-gap: 3rem;
    grid-row-gap: 5rem;
    padding-bottom: 5rem;
`;
const PlotPoints = styled(motion.div)`
    box-sizing: border-box;
    position: relative;
    width: 100%;
    height: 100%;
    display: flex;
    flex-wrap: wrap;
    place-content: flex-start center;
`;

export default LegendDetail;