import { Button } from '@/components/ui/button'
import { fetchMarketChart } from '@/State/Coin/Action'
import { Stethoscope } from 'lucide-react'
import React, { useEffect, useState } from 'react'
import ReactApexChart from 'react-apexcharts'
import { useDispatch, useSelector } from 'react-redux'


const timeSeries = [
    {
        keyword:"DIGITAL_CURRENCY_DAILY",
        key:"Time Series (Daily)",
        lable:"1 Day",
        value:1
    },
    {
        keyword:"DIGITAL_CURRENCY_WEEKLY",
        key:"Weekly Time Series",
        lable:"1 week",
        value:7
    },
    {
        keyword:"DIGITAL_CURRENCY_MONTHLY",
        key:"Monthly Time Series",
        lable:"1 month",
        value:30
    },
    {
        keyword:"DIGITAL_CURRENCY_YEARLY",
        key:"YEARLY Time Series",
        lable:"1 year",
        value:365
    },
]
const StockChart = ({coinId}) => {
    const [activeLable, setActiveLable] = useState(timeSeries[0])
    const [data,setData] = useState([])
    const coin = useSelector(state=>state.coin)
    const dispatch = useDispatch();
    const series = [
        {
            data: coin.marketChart.data || data
        }
    ]


    const option = {
        chart:{
            id:"area-datetime",
            type:"area",
            height:350,
            zoom:{
                autoScaleYaxis:true
            }
        },
        dataLabels:{
            enabled:false
        },
        xaxis:{
            type:"datetime",
            tickAmount:6
        },
        colors:["#758AA2"],
        markers:{
            colors:["#fff"],
            strokeColor:"#fff",
            size:0,
            strokeWith:1,
            strokeDashArray:0,
            style:"hollow",
        },
        tooltip:{
            theme:"dark"
        },
        fill:{
            type:"gradient",
            gradient:{
                shadeIntensity:1,
                opacityFrom:0.5,
                opacityTo:0.9,
                stops:[0,100]
            }
        },
        grid:{
            borderColor:"#47535E",
            strokeDashArray:4,
            show:true
        }
    }
    const handleActiveLable = (value)=>{
        setActiveLable(value)
    }

    useEffect(()=>{
       
            setData(coin.marketChart.data)
            dispatch(fetchMarketChart({coinId,days:activeLable?.value}))
        
    },[activeLable])
  return (
    <div>
        <div className="space-x-3">
            {timeSeries.map(item=><Button variant = {activeLable.value==item.value?"":"outline"} key={item.lable} onClick={()=>handleActiveLable(item)} className={`${activeLable.value==item.value?"":"text-white"}`}>{item.lable}</Button>)}
        </div>
        <div className="chart-timelines">
            
            <ReactApexChart 
            options={option}
            series={series}
            height={350}
            type='area'
            />    
        </div>        
    </div>
  )
}

export default StockChart
