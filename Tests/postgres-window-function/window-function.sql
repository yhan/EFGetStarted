/*******
  POSTGRES WINDOW FUNCTION
  https://www.postgresql.org/docs/current/tutorial-window.html  
  *****/

-- avg salary of the whole company
select "EmpNo", "Salary", avg("Salary") over()
from "EmployeeSalaries"
order by "EmpNo";

--avg
select "DepName", avg("Salary")
from "EmployeeSalaries"
group by "DepName";

-- avg salary of department
select "EmpNo", "Salary", "DepName", avg("Salary") over(partition by "DepName")
from "EmployeeSalaries"
where "DepName" = 'develop';

-- top N salary avg
select "EmpNo", "Salary", "DepName", avg("Salary") over(partition by "DepName" order by "Salary" desc )
from "EmployeeSalaries";


select "EmpNo", "Salary", "DepName", rank() over(partition by "DepName" order by "Salary" desc )
from "EmployeeSalaries";


select "EmpNo", "Salary", "DepName", rank() over(partition by "DepName" order by "Salary" desc ) as pos
from "EmployeeSalaries"
where pos < 3;
-- window function is executed after 'WHERE' clause -> does not compile !
--> correction:

select "EmpNo", "Salary", "DepName", pos
from (select "EmpNo", "Salary", "DepName", rank() over(partition by "DepName" order by "Salary" desc ) as pos
      from "EmployeeSalaries") as ss
where pos < 3
order by "DepName", "Salary" desc;

update "EmployeeSalaries"
set "Salary"= (select "Salary" from "EmployeeSalaries" where "EmpNo" > 30)
where "EmpNo" = 16;
-- ERROR: more than one row returned by a subquery used as an expression


-- Cost by department
select *, sum("Salary") over(partition by "DepName"), avg("Salary") over(partition by "DepName")
from "EmployeeSalaries";

-- Process til current row
select *, sum("Salary") over w, avg("Salary") over w
from "EmployeeSalaries" WINDOW w as (partition by "DepName" order by "Salary" desc );
