# Soda.Pineapple

> Keywords: EfCore, EntityFrameWork, ORM, Sharding, Migration, 分库, 分表, 迁移, 跨库查询

## 功能概述

<table style="width:100%;">
    <tr>
        <th>功能</th><th>详情</th><th>描述</th>
    </tr>
    <tr>
        <td rowspan="3">分库</td><td>相同架构</td><td>类似于多租户的实现</td>
    </tr>
    <tr>
        <td>不同架构</td><td>多库共存</td>
    </tr>
    <tr>
        <td>跨库迁移</td><td>多库迁移</td>
    </tr>
    <tr>
        <td rowspan="2">分表</td><td>分表规则</td><td>根据不同的分表规则, 拆分表</td>
    </tr>
    <tr>
        <td>跨表查询</td><td>多张表跨表查询</td>
    </tr>
</table>

## 架构设计

<div align="center">
    <img src="./doc/images/design.png" style="height:400px"/>
</div>
