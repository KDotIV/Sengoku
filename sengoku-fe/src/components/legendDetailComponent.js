import React from 'react';
//Styling and Animation
import styled from 'styled-components';
import { motion } from "framer-motion";
//Redux
import { useSelector } from 'react-redux';
import { useHistory } from 'react-router-dom';

const LegendDetail = () => {
    const history = useHistory();
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
                            <p key={data.plotId}>{data.text}</p>
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
        width: 100%;
    }
`;

const Summary = styled(motion.div)`
    display: flex;
    align-items: center;
    justify-content: space-between;
`;

const PlotPoints = styled(motion.div)`
    display: flex;
    justify-content: space-evenly;

`;
export default LegendDetail;