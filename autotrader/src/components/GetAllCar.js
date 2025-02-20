import React, { useEffect, useState } from 'react'
import DeleteCar from './DeleteCar'

function GetallCars(props) {
    const url = "https://localhost:7240/cars"
    const [carData, setCardata] = useState([])

        useEffect(() => {
            (async () => {
                const request = await fetch(url, {

                    headers: {
                        'Content-Type' : 'application/json'
                    }
                })

                if(!request.ok){
                    console.log("Hiba")
                    return
                }

                const response = await request.json();
                setCardata(response.result)
                console.log(response.meesage)
            })()
        }, [props.count])

        const carElements = carData.map(
            car =>{
                return(
                    <div class="card m-3 pt-2" style={{'width' : 200, 'float': 'left'}}>
                       <div class="card-header">{car.brand}</div>
                       <div class="card-body">{car.type}</div>
                       <div class="card-body">{car.color}</div>
                       <div class="card-footer">{car.myear}</div>
                       <div><DeleteCar carId = {car.id} handleCount = {props.handleCount}/></div>
                    </div>
                )
            }
        )

    return (
        <div>{carElements}</div>
    )
}

export default GetallCars