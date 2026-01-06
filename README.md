# Travelers guide

## Overview
This project is a desktop application built with **C# and Windows Forms** that allows users to manage a network of cities and roads. Its main goal is to find the **optimal route between two cities** based on **distance, time, or price**.

## Features
- **City Management**
  - Add, edit, and delete cities
  - Each city has a unique name and accessibility status

- **Road Management**
  - Create roads between cities
  - Each road includes distance, travel time, price, and accessibility

- **Route Search**
  - Select start and destination cities
  - Choose an optimization criterion (distance, time, or price)
  - Uses **Dijkstra’s algorithm** to find the optimal route

- **Result Display**
  - Displays the route as a sequence of cities
  - Shows the total value for the selected criterion

## How to Use
1. Add cities using the input field and **Add** button.
2. Add roads by selecting two cities and entering distance, time, and price.
3. Choose start and end cities.
4. Select an optimization criterion.
5. Click **Find Route** to display the result.

## Running the Project
1. **Requirements**
   - Visual Studio 2019 or later
   - .NET Framework 4.7.2 (or compatible)  

2. **Steps**
   1. Clone or download the repository:  
      ```bash
      git clone <your-repo-link>
      ```
   2. Open the `.sln` solution file in **Visual Studio**.
   3. Build the project using **Build > Build Solution**.
   4. Run the application with **Start (F5)**.
   5. Use the interface to add cities, add roads, and find optimal routes.

## Technical Details
- **Vertex class** – represents a city  
- **Edge class** – represents a road between two cities  
- **Graph-based Approach** – the application uses **basic graph concepts**, treating cities as nodes (vertices) and roads as edges, forming a graph structure. This allows the program to compute optimal routes efficiently using **Dijkstra’s algorithm**.  
- **FindShortestPath method** – implements Dijkstra’s algorithm while considering accessibility  

## Future Improvements
- Integration with a real map  
- Online data loading for real roads  
- Extended statistics and network analysis

## Find more details and screenshots from a demonstration of the application in the Документация.docx file

