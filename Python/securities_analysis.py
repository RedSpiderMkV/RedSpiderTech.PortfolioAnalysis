import pandas as pd
import pymysql
from datetime import datetime
from sqlalchemy import create_engine
import matplotlib.pyplot as plt
import numpy

connection_string = 'mysql+pymysql://root:aLLuett3@localhost/SecuritiesResearch'
connection = create_engine(connection_string)

def get_stock_data(symbol, start_date, end_date):
    sql_command = "SELECT sd.*, eod.* FROM EndOfDayData eod" \
        + " JOIN SecurityDetails sd on sd.Id = eod.SecurityId" \
        + " WHERE sd.Symbol = '{0}'".format(symbol) \
        + " AND eod.DateStamp > '{0}'".format(start_date) \
        + " AND eod.DateStamp < '{0}'".format(end_date) \
        + " ORDER by eod.DateStamp ASC"
    data_frame = pd.read_sql(sql_command, con = connection)
    stock_data = [data for index, data in data_frame.iterrows()]
    
    max_close = data_frame["AdjustedClose"].max() * 1.1
    return stock_data, max_close

def plot_data(symbol, start_date, end_date):
    data, max_close = get_stock_data(symbol, start_date, end_date)
    y = [data_point.AdjustedClose for data_point in data]
    x = [data_point.DateStamp for data_point in data]
    plt.plot(x, y, label=symbol)

def plot_all_data(symbol_list):
    for item in symbols:
        plot_data(item)

    plt.title('Stock Movements')
    plt.ylim(0, int(2500))
    plt.legend(bbox_to_anchor=(0, 1), loc='upper left', ncol=1)

def get_closing_data(symbol, start_date, end_date):
    data, meh = get_stock_data(symbol, start_date, end_date)
    closing_data = [x.AdjustedClose for x in data]
    return closing_data

def print_correlation_matrix(symbol_list):
    closing_data_points = { symbol: get_closing_data(symbol) for symbol in symbol_list }
    data_frame = pd.DataFrame(closing_data_points)
    #corr = data_frame.corr()
    #corr.style.background_gradient(cmap='coolwarm')
    print(data_frame.corr())    

def calculate_return(symbol, start_date, end_date):
    closing_data_points = get_closing_data(symbol, start_date, end_date)
    initial_data = closing_data_points[0]
    final_data = closing_data_points[-1]
    return_data = (final_data - initial_data) / initial_data
    
    return return_data * 100

def print_returns(symbol_list, start_date, end_date):
    returns = { symbol: calculate_return(symbol, start_date, end_date) for symbol in symbol_list }
    for key in returns:
        print(key + ": " + str(returns[key]))

#symbols = ["MCRO.L", "EVR.L", "HL.L", "GSK.L", "SSE.L"]
#symbols = ["FB", "AMZN", "AAPL", "NFLX", "GOOG", "MSFT", "^IXIC"] 
#plot_all_data(symbols)
#print_correlation_matrix(symbols)

symbols = ["FB", "AMZN", "AAPL", "NFLX", "GOOG", "MSFT", "^IXIC"] 
#symbols = ["HSBA.L", "BARC.L", "EVR.L", "SSE.L", "HL.L", "GSK.L", "MCRO.L"] 
start_date = "2019-09-01"
end_date = "2019-09-30"
#plot_all_data(symbols)
#print_correlation_matrix(symbols)
print_returns(symbols, start_date, end_date)
